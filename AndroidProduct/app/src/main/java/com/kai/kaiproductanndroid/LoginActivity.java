package com.kai.kaiproductanndroid;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

import androidx.appcompat.app.AppCompatActivity;
import com.auth0.android.Auth0;
import com.auth0.android.provider.WebAuthProvider;
import com.auth0.android.result.Credentials;
import com.auth0.android.callback.Callback;
import com.auth0.android.authentication.AuthenticationException;

public class LoginActivity extends AppCompatActivity {

    private Auth0 auth0;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        Button btnLogin = findViewById(R.id.btnLogin);
        Button btnLogout = findViewById(R.id.btnLogout);
        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Login();
            }
        });
        btnLogout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Logout();
            }
        });

    }

    private void Login()
    {
        auth0 = new Auth0(
                "8bU8ZPxxOHe3s4JAemnh5U7oXpvmEFNC",   // 從 Auth0 控制台取得
                "dev-ls2a6osj1qusguy7.us.auth0.com"       // 例如：your-app.auth0.com
        );

        WebAuthProvider.login(auth0)
                .withScheme("com.kai.kaiproductanndroid")  // 與 manifest & Auth0 callback 保持一致
                .withScope("openid profile email")
                .start(this, new Callback<Credentials, AuthenticationException>() {
                    @Override
                    public void onSuccess(Credentials credentials) {
                        Log.d("Auth0", "AccessToken: " + credentials.getAccessToken());
                        // 在這裡你可以呼叫 /userinfo 或其他 API
                    }

                    @Override
                    public void onFailure(AuthenticationException e) {
                        Log.e("Auth0", "Login failed", e);
                    }
                });
    }

    private void Logout()
    {
        WebAuthProvider.logout(auth0)
                .withScheme("com.kai.kaiproductanndroid") // 跟你 manifest 及 login 時用的一樣
                .start(this, new Callback<Void, AuthenticationException>() {
                    @Override
                    public void onSuccess(Void payload) {
                        // 登出成功，清除本地資料（例如 SharedPreferences 的 token）
                        // 可以導回登入頁面或主頁
                        runOnUiThread(() -> {
                            // 例如跳轉登入頁
                        });
                    }

                    @Override
                    public void onFailure(AuthenticationException error) {
                        // 登出失敗，處理錯誤
                    }
                });
    }

}