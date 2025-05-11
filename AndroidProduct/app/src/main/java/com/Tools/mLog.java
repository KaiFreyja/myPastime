package com.Tools;

import android.graphics.Bitmap;
import android.util.Log;

import java.io.File;

public class mLog
{
    private static final String TAG = "over";

    public static void d(String text) {
        if (GlobalData.IS_LOG()) {
            Log.d(TAG, text + "\n" + getOri());
        }
    }
    public static void e(String text) {
        Log.d(TAG ,text + "\n" + getOri());
    }

    public static void e(Exception exception) {
        Log.d(TAG , exception.toString() + "\n" + getOri());
        exception.printStackTrace();
    }

    public static void d(Bitmap bitmap) {
        if (GlobalData.IS_LOG()) {
            String s = "bitmap : ";
            if (bitmap != null) {
                s += "(w,h) = ( " + bitmap.getWidth() + " , " + bitmap.getHeight() + " )" + "\n";
            } else {
                s += "NULL";
            }
            Log.d(TAG, s + "\n" + getOri());
        }
    }

    public static void d(File file) {
        if (GlobalData.IS_LOG()) {
            StringBuilder folderStructure = new StringBuilder();
            // 调用递归方法构建文件夹结构字符串
            buildFolderStructure(file, 0, folderStructure);
            String text = folderStructure.toString();
            Log.d(TAG, text + "\n" + getOri());
        }
    }

    StringBuilder folderStructure = new StringBuilder();

    // 递归方法，用于构建文件夹结构字符串
    public static void buildFolderStructure(File folder, int depth, StringBuilder folderStructure) {
        // 添加当前文件夹的名称到结构字符串中
        appendIndent(depth, folderStructure);
        folderStructure.append(folder.getName()).append("=>").append((folder.length()/ 1024.0) + " KB").append("\n");

        // 获取文件夹中的所有文件和文件夹
        File[] files = folder.listFiles();
        if (files != null) {
            // 遍历文件夹中的每个文件和文件夹
            for (File file : files) {
                // 如果是文件夹，则递归调用 buildFolderStructure 方法构建其中的内容
                if (file.isDirectory()) {
                    buildFolderStructure(file, depth + 1, folderStructure);
                } else {
                    // 如果是文件，则添加文件名称到结构字符串中
                    appendIndent(depth + 1, folderStructure);
                    folderStructure.append(file.getName()).append("=>").append((file.length()/ 1024.0) + " KB").append("\n");
                }
            }
        }
    }

    // 辅助方法，用于添加缩进到结构字符串中
    public static void appendIndent(int depth, StringBuilder folderStructure) {
        for (int i = 0; i < depth; i++) {
            folderStructure.append("  "); // 使用两个空格作为缩进
        }
    }

    private static final int COUNT_STACK_TRACE = 8;
    private static final int START = 4;

    private static String getOri() {
        StackTraceElement[] stackTraceElements = Thread.currentThread().getStackTrace();
        int start = START;
        int count = COUNT_STACK_TRACE + start;
        count = stackTraceElements.length > count ? count : stackTraceElements.length - 1;
        String result = "";
        for (int i = start;i < count;i++) {
            StackTraceElement caller = stackTraceElements[i];
            result += "\n->" + caller.getClassName();
            result += "->>" + caller.getMethodName();
        }
        return result;
    }
}
