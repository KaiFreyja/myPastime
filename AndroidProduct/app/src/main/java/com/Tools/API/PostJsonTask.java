package com.Tools.API;

import android.os.AsyncTask;
import android.util.Log;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

public class PostJsonTask extends AsyncTask<Void, Void, JSONObject> {

    private static final String TAG = "PostJsonTask";
    private String apiUrl;
    private JSONObject postData;
    private BaseAPICallBack callBack;

    public PostJsonTask(String apiUrl, JSONObject postData, BaseAPICallBack callBack) {
        this.apiUrl = apiUrl;
        this.postData = postData;
        this.callBack = callBack;
    }

    @Override
    protected JSONObject doInBackground(Void... voids) {
        HttpURLConnection urlConnection = null;
        BufferedReader reader = null;

        try {
            URL url = new URL(apiUrl);
            urlConnection = (HttpURLConnection) url.openConnection();

            urlConnection.setRequestMethod("POST");
            urlConnection.setRequestProperty("Content-Type", "application/json; charset=UTF-8");
            urlConnection.setDoOutput(true);
            urlConnection.setDoInput(true);

            OutputStream os = urlConnection.getOutputStream();
            os.write(postData.toString().getBytes("UTF-8"));
            os.close();

            InputStream inputStream = urlConnection.getInputStream();
            if (inputStream == null) {
                return null;
            }

            reader = new BufferedReader(new InputStreamReader(inputStream));
            StringBuilder buffer = new StringBuilder();
            String line;
            while ((line = reader.readLine()) != null) {
                buffer.append(line).append("\n");
            }

            if (buffer.length() == 0) {
                return null;
            }

            String jsonString = buffer.toString();
            Log.d(TAG, "JSON Response: " + jsonString);
            return new JSONObject(jsonString);

        } catch (Exception e) {
            Log.e(TAG, "Error in POST API: ", e);
            return null;
        } finally {
            if (urlConnection != null) {
                urlConnection.disconnect();
            }
            if (reader != null) {
                try {
                    reader.close();
                } catch (IOException e) {
                    Log.e(TAG, "Error closing stream: ", e);
                }
            }
        }
    }

    @Override
    protected void onPostExecute(JSONObject jsonObject) {
        callBack.CallBack(new APIResult() {
            @Override
            public JSONObject GetData() {
                return jsonObject;
            }
        });
    }
}
