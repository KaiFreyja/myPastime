package com.Tools;

import android.app.Activity;
import android.app.Dialog;
import android.content.DialogInterface;
import taimoor.sultani.sweetalert2.Sweetalert;
public class mAlert {
    public static Activity activity;

    public static Dialog showError(String title, String message
            , String btn_1_text, DialogInterface.OnClickListener btn_1_listener
            , String btn_2_text, DialogInterface.OnClickListener btn_2_listener) {
        Sweetalert alertDialog = new Sweetalert(activity, Sweetalert.WARNING_TYPE);
        alertDialog.setTitleText(title)
                .setContentText(message)
                .setConfirmButton(btn_1_text, new Sweetalert.OnSweetClickListener() {
                    @Override
                    public void onClick(Sweetalert sDialog) {
                        btn_1_listener.onClick(sDialog, 0);
                        sDialog.dismissWithAnimation();
                    }
                })
                .setCancelButton(btn_2_text, new Sweetalert.OnSweetClickListener() {
                    @Override
                    public void onClick(Sweetalert sDialog) {
                        btn_2_listener.onClick(sDialog, 1);
                        sDialog.dismissWithAnimation();
                    }
                })
                .showConfirmButton(true)
                .showCancelButton(true)
                .show();
        return alertDialog;

/*
        // 创建警报对话框构建器
        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
        // 设置对话框标题和消息
        builder.setTitle(title);
        builder.setMessage(message);
        // 设置按钮及其点击事件
        builder.setPositiveButton(btn_1_text, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                btn_1_listener.onClick(dialog,which);
                dialog.dismiss();
            }
        });
        builder.setNegativeButton(btn_2_text, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                btn_2_listener.onClick(dialog,which);
                dialog.dismiss();
            }
        });
        // 创建并显示警报对话框
        AlertDialog alertDialog = builder.create();
        alertDialog.show();

        return alertDialog;
        */
    }

    public static Dialog showError(String title, String message) {
        mLog.d("alert error : " + title + "->" + message);

        /*
        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
        builder.setTitle(title);
        builder.setMessage(message);
        AlertDialog alertDialog = builder.create();
        alertDialog.show();
        */
        /*
        new Sweetalert(activity, Sweetalert.SUCCESS_TYPE)
                .setTitleText(title)
                .setContentText(message)
                .show();*/
        Sweetalert alertDialog = new Sweetalert(activity, Sweetalert.ERROR_TYPE);
        alertDialog.setTitleText(title)
                .setContentText(message)
                .setConfirmButton("確定", new Sweetalert.OnSweetClickListener() {
                    @Override
                    public void onClick(Sweetalert sweetAlertDialog) {
                        sweetAlertDialog.dismiss();
                    }
                })
                .showConfirmButton(true)
                .show();
        return alertDialog;
    }
    public static Dialog show(String title, String message) {
        mLog.d("alert error : " + title + "->" + message);

        /*
        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
        builder.setTitle(title);
        builder.setMessage(message);
        AlertDialog alertDialog = builder.create();
        alertDialog.show();
        */
        /*
        new Sweetalert(activity, Sweetalert.SUCCESS_TYPE)
                .setTitleText(title)
                .setContentText(message)
                .show();*/
        Sweetalert alertDialog = new Sweetalert(activity, Sweetalert.SUCCESS_TYPE);
        alertDialog.setTitleText(title)
                .setContentText(message)
                .setConfirmButton("確定", new Sweetalert.OnSweetClickListener() {
                    @Override
                    public void onClick(Sweetalert sweetAlertDialog) {
                        sweetAlertDialog.dismiss();
                    }
                })
                .showConfirmButton(true)
                .show();
        return alertDialog;
    }
}
