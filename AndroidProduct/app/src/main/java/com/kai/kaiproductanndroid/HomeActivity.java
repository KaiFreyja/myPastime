package com.kai.kaiproductanndroid;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class HomeActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home);

        Button btnJump = findViewById(R.id.btnjump);
        btnJump.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(HomeActivity.this,RoleListActivity.class);
                startActivity(intent);
            }
        });

        Button btn = findViewById(R.id.btnApi);
        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(HomeActivity.this,GachaponActivity.class);
                startActivity(intent);
            }
        });

        Button btnLogin = findViewById(R.id.btnLogin);
        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(HomeActivity.this,FirebaseLoginActivity.class);
                startActivity(intent);
            }
        });

        Button btnProfession = findViewById(R.id.btnProfession);
        btnProfession.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(HomeActivity.this,ProfessionATkRateActivity.class);
                startActivity(intent);
            }
        });

        Button btnMapPos = findViewById(R.id.btnMapPos);
        btnMapPos.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(HomeActivity.this,MapPosActivity.class);
                startActivity(intent);
            }
        });

        Button btnAiTalk = findViewById(R.id.btn_ai_talk);
        btnAiTalk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(HomeActivity.this, AiTalkMainActivity.class);
                startActivity(intent);
            }
        });
    }
}