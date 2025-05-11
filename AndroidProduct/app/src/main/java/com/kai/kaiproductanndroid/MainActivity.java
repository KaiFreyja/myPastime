package com.kai.kaiproductanndroid;

import android.content.Intent;
import android.os.Bundle;

import androidx.appcompat.app.AppCompatActivity;

import android.view.View;

import androidx.navigation.ui.AppBarConfiguration;

import com.Tools.API.APIResult;
import com.Tools.API.BaseAPICallBack;
import com.kai.kaiproductanndroid.databinding.ActivityMainBinding;

import android.view.Menu;
import android.view.MenuItem;
import android.widget.Button;
import android.widget.TextView;

import org.json.JSONObject;

public class MainActivity extends AppCompatActivity {

    private AppBarConfiguration appBarConfiguration;
    private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        setSupportActionBar(binding.toolbar);

        Button btnJump = findViewById(R.id.btnjump);
        btnJump.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this,RoleListActivity.class);
                startActivity(intent);
            }
        });

        TextView tv = findViewById(R.id.textView);
        Button btn = findViewById(R.id.btnApi);
        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                APIController controller = new APIController();
                controller.GetFgoGetRole(new JSONObject(), new BaseAPICallBack() {
                    @Override
                    public void CallBack(APIResult result) {
                        JSONObject json = result.GetData();
                        String text = "null";
                        if(json != null)
                        {
                            text = json.toString();
                        }
                        tv.setText(text);
                    }
                });
            }
        });

        Button btnGender = findViewById(R.id.btnGender);
        btnGender.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                APIController controller = new APIController();
                controller.GetFgoGetGender(new JSONObject(), new BaseAPICallBack() {
                    @Override
                    public void CallBack(APIResult result) {
                        JSONObject json = result.GetData();
                        String text = "null";
                        if(json != null)
                        {
                            text = json.toString();
                        }
                        tv.setText(text);
                    }
                });
            }
        });

        Button btnProfession = findViewById(R.id.btnProfession);
        btnProfession.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this,ProfessionATkRateActivity.class);
                startActivity(intent);
                /*
                APIController controller = new APIController();
                controller.GetFgoGetProfession(new JSONObject(), new BaseAPICallBack() {
                    @Override
                    public void CallBack(APIResult result) {
                        JSONObject json = result.GetData();
                        String text = "null";
                        if(json != null)
                        {
                            text = json.toString();
                        }
                        tv.setText(text);
                    }
                });*/
            }
        });

        Button btnMapPos = findViewById(R.id.btnMapPos);
        btnMapPos.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this,MapPosActivity.class);
                startActivity(intent);
            }
        });

        Button btnAiTalk = findViewById(R.id.btn_ai_talk);
        btnAiTalk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, AiTalkMainActivity.class);
                startActivity(intent);
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
}