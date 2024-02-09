using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance {get; private set;}

    [Header(" Firebase ")]
    public DependencyStatus FBDependencyStatus;

    [Header(" Firestore ")]
    private FirebaseFirestore _firebaseFirestore;

    [Header(" Login ")]
    private FirebaseAuth _firebaseAuth;
    private FirebaseUser _firebaseUser;
    private string _firebaseUserID;

    [Header(" Actions ")]
    public static Action OnLogged;
    public static Action OnFirst;

    private void Awake()
    {
        SingletonObject();
    }

    private void Start()
    {
        StartCoroutine(InitializeFirebase());
    }

    private void SingletonObject()
    { 
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #region Initialize Firebase
    private IEnumerator InitializeFirebase()
    {
        var dependencyStatus = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(()=> dependencyStatus.IsCompleted);

        FBDependencyStatus = dependencyStatus.Result;

        if(FBDependencyStatus == DependencyStatus.Available)
        {
            _firebaseAuth = FirebaseAuth.DefaultInstance;
            _firebaseAuth.StateChanged += AuthStateChanged;
            AuthStateChanged(this,null);
            _firebaseFirestore = FirebaseFirestore.DefaultInstance;
            
            Debug.Log("Success");
        }
        else
        {
            Debug.LogError("Could not resolve all firebase dependencies: " + FBDependencyStatus);
        }

        yield return StartCoroutine(CheckForAutoLogin());
    }
    #endregion

    #region Firestore
    public IEnumerator CreateUser(Data data)
    {
        if(_firebaseUser == null)
        yield break;

        CollectionReference usersRef = _firebaseFirestore.Collection("users");

        var task = usersRef.Document(_firebaseUser.UserId).SetAsync(data.ToDictionary());
        yield return new WaitUntil(()=> task.IsCompleted);

        if(task.Exception != null)
        {
            Debug.LogError(message: $"Failed to register task with {task.Exception}");
        }
        else
        {
            Debug.Log("Complete Save Data.");
        }
    }

    public IEnumerator ReadUser(Action<Data> OnRead)
    {
        CollectionReference usersRef = _firebaseFirestore.Collection("users");

        DocumentReference usersDocumentReference = usersRef.Document(_firebaseUser.UserId);

        var readTask = usersDocumentReference.GetSnapshotAsync();

        yield return new WaitUntil(()=> readTask.IsCompleted);

        if(readTask.Exception != null)
        {
            Debug.LogError(readTask.Exception);
        }
        else
        {
            DocumentSnapshot snapshot = readTask.Result;

            if(snapshot.Exists)
            {
                Data data = new Data(snapshot);
                OnRead?.Invoke(data);
            }
            else
            {
                Debug.LogError("User not found");
            }
        }
    }
    
    public IEnumerator GetAllUsers(Action<List<Data>> callback)
    {
        CollectionReference usersRef = _firebaseFirestore.Collection("users");
        // All users data
        var queryTask = usersRef.Limit(50).GetSnapshotAsync();

        yield return new WaitUntil(()=> queryTask.IsCompleted);

        if(queryTask.Exception != null)
            Debug.LogError(queryTask.Exception);
        else
        {
            List<Data> userList = new List<Data>();
            QuerySnapshot snapshot = queryTask.Result;

            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                Data data = new Data(documentSnapshot);
                userList.Add(data);
            }
            // TotalCps'e göre sırala
            userList = userList.OrderByDescending(x => x.TotalCps).ToList();
            callback?.Invoke(userList);
        }
    }
    
    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(_firebaseAuth.CurrentUser !=  _firebaseUser)
        {
            bool signedIn = _firebaseUser != _firebaseAuth.CurrentUser && _firebaseAuth.CurrentUser != null;

            if (!signedIn && _firebaseUser != null)
            {
                Debug.Log("Signed out " + _firebaseUser.UserId);
            }
            _firebaseUser = _firebaseAuth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed In " + _firebaseUser.UserId);
            }
        }

        
    }
    
    #endregion

    #region  Email - Pass
    #region Login
    public IEnumerator LoginAsync(string email, string password, Action<string> OnFailure)
    {
        var loginTask = _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(()=> loginTask.IsCompleted);

        if(loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Failed! Reason: ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Invalid email.";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong password.";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing.";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing.";
                    break;
                case AuthError.UserNotFound:
                    failedMessage += "User account not found.";
                    break;
                default:
                    failedMessage += "Unknown error.";
                    break;
            }
            OnFailure?.Invoke(failedMessage);
        }
        else
        {
            AuthResult result = loginTask.Result;
            if(result.User != null)
            {
                _firebaseUser = result.User;
                _firebaseUserID = _firebaseUser.UserId;
                OnLogged?.Invoke(); 
            }
        }
    }
    #endregion

    #region Register
    public IEnumerator RegisterAsync(string email, string password, string nickname, Action OnRegister, Action<string> OnFailure)
    {
        var registerTask = _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email,password);

        yield return new WaitUntil(()=> registerTask.IsCompleted);

        if(registerTask.Exception != null)
        {
            Debug.LogError(registerTask.Exception);

            FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Registration Failed! Because ";
            switch(authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid.";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password.";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing.";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing.";
                    break;
                case AuthError.EmailAlreadyInUse:
                    failedMessage += "Email is already in use.";
                    break;
                default:
                    failedMessage += "Registration Failed.";
                    break;
            }
            Debug.Log(failedMessage);

            // Error:
            OnFailure?.Invoke(failedMessage);
        }
        else
        {
            AuthResult result = registerTask.Result;
            _firebaseUser = result.User;
            _firebaseUserID = _firebaseUser.UserId;

            Data data = new Data();
            data.Nickname = nickname;

            yield return StartCoroutine(CreateUser(data));
            Debug.Log("Registration Successful Welcome " + _firebaseUser.Email);
        }
    }
    #endregion
    #endregion

    #region Anonymous
    public IEnumerator AnonymouseLogin(string nickname, Action<string> OnFailure)
    {
        bool isAvailableNickname;
        var checkNicknameTask = CheckDisplayNameAvailability(nickname);

        yield return new WaitUntil(() => checkNicknameTask.IsCompleted);        

        if(checkNicknameTask.Exception != null)
        {
            Debug.LogError(checkNicknameTask.Exception);
            yield break;
        }

        isAvailableNickname = checkNicknameTask.Result;

        if (isAvailableNickname)
        {
            // DisplayName is available, continue with login process
            _firebaseAuth.SignOut(); // Silinecek
            var task = _firebaseAuth.SignInAnonymouslyAsync();

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
                yield break;
            }

            AuthResult result = task.Result;

            if (result.User == null)
            {
                OnFailure?.Invoke("Error");
                yield break;
            }
            _firebaseUser = result.User;            
            _firebaseUserID = result.User.UserId;

            UserProfile userProfile = new UserProfile { DisplayName = nickname };

            var updateProfileTask = _firebaseUser.UpdateUserProfileAsync(userProfile);

            yield return new WaitUntil(()=> updateProfileTask.IsCompleted);

            if(updateProfileTask.Exception != null)
            {
                OnFailure(updateProfileTask.Exception.ToString());
                yield break;
            }

            PlayerPrefs.SetString("Id", _firebaseUserID);
            Debug.Log("Welcome " + _firebaseUser.DisplayName);
            Data data = new Data();
            data.Nickname = _firebaseUser.DisplayName;
            yield return StartCoroutine(CreateUser(data));

            OnLogged?.Invoke();
        }
        else
        {
            string failedMessage = "Nickname already in use.";
            Debug.LogError(failedMessage);
            OnFailure?.Invoke(failedMessage);
        }

    }

    private async Task<bool> CheckDisplayNameAvailability(string displayName)
    {
        var usersRef = _firebaseFirestore.Collection("users");
        var query = usersRef.WhereEqualTo("Nickname", displayName).Limit(1);

        var querySnapshotTask = query.GetSnapshotAsync();

        await querySnapshotTask;

        if (querySnapshotTask.Exception != null)
        {
            Debug.LogError(querySnapshotTask.Exception);
            return false;
        }
        Debug.Log(querySnapshotTask.Result.Count);
        return querySnapshotTask.Result.Count == 0;
    }
    #endregion

    #region Auto Login
    private IEnumerator CheckForAutoLogin()
    {
        if(_firebaseUser != null)
        {
            var reloadUserTask = _firebaseUser.ReloadAsync();

            yield return new WaitUntil(()=> reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else
        {
           OnFirst?.Invoke();
        }
    }

    private void AutoLogin()
    {
        if(_firebaseUser != null)
        {
            OnLogged?.Invoke();
        }
        else
        {
            OnFirst?.Invoke();
        }
    }
    #endregion
}
