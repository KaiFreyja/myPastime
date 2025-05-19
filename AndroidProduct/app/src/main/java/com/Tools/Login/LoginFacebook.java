package com.Tools.Login;

import android.app.Activity;
import android.content.Intent;

import com.Tools.mLog;
import com.facebook.AccessToken;
import com.facebook.CallbackManager;
import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.FacebookSdk;
import com.facebook.login.LoginManager;
import com.google.firebase.auth.AuthCredential;
import com.google.firebase.auth.FacebookAuthProvider;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;

import java.util.Arrays;

public class LoginFacebook implements ILogin
{
    Activity activity;
    // 宣告 callbackManager
    private CallbackManager callbackManager;

    private LoginCallback callback;
    @Override
    public void Init(Activity activity) {
        this.activity = activity;
        FacebookSdk.sdkInitialize(activity.getApplicationContext());
        callbackManager = CallbackManager.Factory.create();
    }

    @Override
    public void Login(LoginCallback callback) {
        this.callback = callback;
        LoginManager.getInstance().logInWithReadPermissions(activity, Arrays.asList("email", "public_profile"));
        LoginManager.getInstance().registerCallback(callbackManager, new FacebookCallback<com.facebook.login.LoginResult>() {
            @Override
            public void onSuccess(com.facebook.login.LoginResult loginResult) {
                handleFacebookAccessToken(loginResult.getAccessToken());
            }
            @Override
            public void onCancel() {
                // 使用者取消登入
                if(callback != null)
                {
                    LoginResult result = new LoginResult();
                    result.isSuccess = false;
                    callback.LoginFinish(result);
                }
            }
            @Override
            public void onError(FacebookException error) {
                // 登入出錯
                if(callback != null)
                {
                    LoginResult result = new LoginResult();
                    result.isSuccess = false;
                    callback.LoginFinish(result);
                }
            }
        });
    }

    // 處理 Facebook Token 並登入 Firebase
    private void handleFacebookAccessToken(AccessToken token) {
        AuthCredential credential = FacebookAuthProvider.getCredential(token.getToken());
        FirebaseAuth.getInstance().signInWithCredential(credential)
                .addOnCompleteListener(activity, task -> {
                    FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
                    // 登入成功，更新 UI 或跳轉頁面
                    LoginResult result = new LoginResult();
                    result.isSuccess = true;
                    result.oid = user.getUid();
                    result.name = user.getDisplayName();
                    result.phone = user.getPhoneNumber();
                    result.email = user.getEmail();
                    if(callback != null)
                    {
                        callback.LoginFinish(result);
                    }
                });
    }

    @Override
    public void Signout() {

    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        callbackManager.onActivityResult(requestCode, resultCode, data);
    }
}
