package com.kai.kaiproductanndroid;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import androidx.appcompat.app.AppCompatActivity;

import com.Tools.API.APIResult;
import com.Tools.API.BaseAPICallBack;
import com.Tools.Login.ILogin;
import com.Tools.Login.LoginCallback;
import com.Tools.Login.LoginFacebook;
import com.Tools.Login.LoginGoogle;
import com.Tools.Login.LoginResult;
import com.Tools.mLog;

import org.json.JSONException;
import org.json.JSONObject;

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
                    int ltid = 0;
                    if (login instanceof LoginGoogle) {
                        ltid = 1;
                    } else if (login instanceof LoginFacebook) {
                        ltid = 2;
                    }

                    if (ltid == 0) {
                        mLog.d("no this third Login");
                        return;
                    }


                    JSONObject input = new JSONObject();
                    try {
                        input.put("oid", result.oid);
                        input.put("ltid", ltid);
                    } catch (JSONException e) {
                        mLog.e(e);
                    }

                    APIController controller = new APIController();
                    controller.LoginThird(input, new BaseAPICallBack() {
                        @Override
                        public void CallBack(APIResult result) {
                            JSONObject json = result.GetData();
                            try {
                                JSONObject login_third = json.getJSONObject("login_third");
                                GlobalData.uid = login_third.getInt("uid");
                                mLog.d("登入" + login_third);
                                gotoHome();
                            } catch (JSONException e) {
                                mLog.e(e);
                            }
                        }
                    });
                }
                else
                {
                    mLog.d("no login");
                }
            }
        });
    }

    private  void  gotoHome()
    {
        Intent intent = new Intent(FirebaseLoginActivity.this,HomeActivity.class);
        startActivity(intent);
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



