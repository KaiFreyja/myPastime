package com.kai.kaiproductanndroid;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.animation.ValueAnimator;
import android.os.Bundle;
import android.provider.Settings;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;

import com.Tools.API.APIResult;
import com.Tools.API.BaseAPICallBack;
import com.Tools.mLog;
import com.airbnb.lottie.LottieAnimationView;
import com.squareup.picasso.Picasso;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class GachaponActivity extends AppCompatActivity {

    LottieAnimationView lottieView;
    LottieAnimationView lottieView2;
    LottieAnimationView lottieView3;
    ValueAnimator speedAnimeror;
    ImageView img_card;

    ArrayList<JSONObject> items = new ArrayList<>();
    int flag = 0;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_gachapon);


        AnimationSetting();

        Button btn_gachapon_one = findViewById(R.id.btn_gachapon_one);
        btn_gachapon_one.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                gachaponOne();
            }
        });

        Button btn_gachapon_ten = findViewById(R.id.btn_gachapon_ten);
        btn_gachapon_ten.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                gachaponTen();
            }
        });
    }
    private void gachaponTen()
    {
        items = new ArrayList<>();
        JSONObject input = new JSONObject();
        try {
            input.put("uid",GlobalData.uid);
            input.put("ggid","1");
        } catch (JSONException e) {
            mLog.e(e);
        }
        APIController controller = new APIController();
        controller.SendGachaponTen(input, new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                var json = result.GetData();
                try {
                    var gachapon_ten = json.getJSONArray("gachapon_ten");
                    for (int i = 0;i < gachapon_ten.length();i++)
                    {
                        items.add(gachapon_ten.getJSONObject(i));
                    }
                } catch (JSONException e) {
                    mLog.e(e);
                }
                flag = 0;
                AnimationPlay();
            }
        });
    }
    private void gachaponOne()
    {
        items = new ArrayList<>();
        JSONObject input = new JSONObject();
        try {
            input.put("uid",GlobalData.uid);
            input.put("ggid","1");
        } catch (JSONException e) {
            mLog.e(e);
        }
        APIController controller = new APIController();
        controller.SendGachaponOne(input, new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                var json = result.GetData();
                try {
                    var gachapon_one = json.getJSONObject("gachapon_one");
                    items.add(gachapon_one);
                } catch (JSONException e) {
                    mLog.e(e);
                }
                flag = 0;
                AnimationPlay();
            }
        });
    }

    private void  AnimationPlayFinsh()
    {
        flag++;
        if(flag >= items.size())
        {
            return;
        }

        AnimationPlay();
    }

    private void  AnimationPlay()
    {
        lottieView.setVisibility(View.VISIBLE);
        lottieView2.setVisibility(View.INVISIBLE);
        lottieView3.setVisibility(View.INVISIBLE);
        img_card.setVisibility(View.INVISIBLE);

        JSONObject item = items.get(flag);
        String imageUrl = null;
        try {
            imageUrl = item.getString("url");
        } catch (JSONException e) {
            mLog.e(e);
        }
        Picasso.get()
                .load(imageUrl)
                .into(img_card);

        lottieView.playAnimation();
        speedAnimeror.start();
    }

    private void AnimationSetting()
    {
        lottieView = findViewById(R.id.lottieView);
        lottieView2 = findViewById(R.id.lottieView2);
        lottieView3 = findViewById(R.id.lottieView3);
        img_card = findViewById(R.id.img_card);

        lottieView2.setVisibility(View.INVISIBLE);
        lottieView3.setVisibility(View.INVISIBLE);
        img_card.setVisibility(View.INVISIBLE);


        speedAnimeror = ValueAnimator.ofFloat(0.5f,10.0f);
        speedAnimeror.setDuration(1500);
        speedAnimeror.addUpdateListener(new ValueAnimator.AnimatorUpdateListener() {
            @Override
            public void onAnimationUpdate(ValueAnimator animation) {
                float speed = (float) animation.getAnimatedValue();
                if(speed >= 5 && !lottieView2.isAnimating())
                {
                    lottieView2.setVisibility(View.VISIBLE);
                    lottieView2.playAnimation();
                    lottieView2.setSpeed(2f);
                }
                lottieView.setSpeed(speed);
            }
        });

        lottieView2.addAnimatorListener(new AnimatorListenerAdapter() {
            @Override
            public void onAnimationEnd(Animator animation) {
                super.onAnimationEnd(animation);
                lottieView2.setVisibility(View.INVISIBLE);
                lottieView3.setVisibility(View.VISIBLE);
                img_card.setVisibility(View.VISIBLE);
                lottieView3.playAnimation();
            }
        });

        lottieView3.addAnimatorListener(new AnimatorListenerAdapter() {
            @Override
            public void onAnimationEnd(Animator animation) {
                super.onAnimationEnd(animation);
                AnimationPlayFinsh();
            }
        });
    }
}