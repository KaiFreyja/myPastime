package com.Tools.API;

import android.os.AsyncTask;
import android.util.Log;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

public class FetchJsonTask extends AsyncTask<Void, Void, JSONObject> {

    private static final String TAG = "FetchJsonTask";
    private String apiUrl;
    private BaseAPICallBack callBack;
    public FetchJsonTask(String apiUrl,BaseAPICallBack callBack) {
        this.apiUrl = apiUrl;
        this.callBack = callBack;

    }

    @Override
    protected JSONObject doInBackground(Void... voids) {

        HttpURLConnection urlConnection = null;
        BufferedReader reader = null;

        try {
            URL url = new URL(apiUrl);
            urlConnection = (HttpURLConnection) url.openConnection();
            urlConnection.setRequestMethod("GET");
            urlConnection.connect();

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

        } catch (IOException e) {
            Log.e(TAG, "Error fetching data: ", e);
            return null;
        } catch (JSONException e) {
            Log.e(TAG, "Error parsing JSON: ", e);
            return null;
        } finally {
            if (urlConnection != null) {
                urlConnection.disconnect();
            }
            if (reader != null) {
                try {
                    reader.close();
                } catch (final IOException e) {
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



