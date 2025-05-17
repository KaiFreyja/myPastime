package com.kai.kaiproductanndroid;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import androidx.appcompat.app.AppCompatActivity;

import com.Tools.Login.ILogin;
import com.Tools.Login.LoginCallback;
import com.Tools.Login.LoginFacebook;
import com.Tools.Login.LoginGoogle;
import com.Tools.Login.LoginResult;
import com.Tools.mLog;

public class FirebaseLoginActivity extends AppCompatActivity {
    ILogin login;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_firebase_login);

        Button btnLoginGoogle = findViewById(R.id.btnLoginGoogle);
        btnLoginGoogle.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                login = new LoginGoogle();
                StartLogin();
            }
        });

        Button btnLoginFacebook = findViewById(R.id.btnLoginFacebook);
        btnLoginFacebook.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                login = new LoginFacebook();
                StartLogin();
            }
        });
    }

    private void StartLogin()
    {
        login.Init(FirebaseLoginActivity.this);
        login.Login(new LoginCallback() {
            @Override
            public void LoginFinish(LoginResult result) {
                if (result.isSuccess) {
                    mLog.d("result.oid : " + result.oid);
                    mLog.d("result.name : " + result.name);
                }
                else
                {
                    mLog.d("no login");
                }
            }
        });
    }

    @Override
    public void onStart() {
        super.onStart();
    }


    // 在 activity 裡覆寫 onActivityResult，交給 Facebook SDK 處理
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (login == null)
            return;
        login.onActivityResult(requestCode, resultCode, data);
    }
}



