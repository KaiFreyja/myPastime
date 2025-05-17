package com.Tools.Login;

import android.app.Activity;
import android.content.Intent;

public interface ILogin {
    void Init(Activity activity);

    void Login(LoginCallback callback);

    void Signout();

    void  onActivityResult(int requestCode, int resultCode, Intent data);
}
