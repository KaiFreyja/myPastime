package com.kai.kaiproductanndroid;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.animation.ValueAnimator;
import android.os.Bundle;
import android.view.View;

import com.airbnb.lottie.LottieAnimationView;

public class GachaponActivity extends AppCompatActivity {

    LottieAnimationView lottieView;
    LottieAnimationView lottieView2;
    LottieAnimationView lottieView3;
    ValueAnimator speedAnimeror;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_gachapon);

        lottieView = findViewById(R.id.lottieView);
        lottieView2 = findViewById(R.id.lottieView2);
        lottieView3 = findViewById(R.id.lottieView3);

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
                lottieView3.playAnimation();
            }
        });

        lottieView3.addAnimatorListener(new AnimatorListenerAdapter() {
            @Override
            public void onAnimationEnd(Animator animation) {
                super.onAnimationEnd(animation);
                AnimationPlay();
            }
        });

        AnimationPlay();
    }

    private void  AnimationPlay()
    {
        lottieView.setVisibility(View.VISIBLE);
        lottieView2.setVisibility(View.INVISIBLE);
        lottieView3.setVisibility(View.INVISIBLE);
        lottieView.playAnimation();
        speedAnimeror.start();
    }
}