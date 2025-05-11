package com.kai.kaiproductanndroid;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;

import com.Tools.API.APIResult;
import com.Tools.API.BaseAPICallBack;
import com.Tools.mLog;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class MapPosActivity extends AppCompatActivity implements OnMapReadyCallback {

    private GoogleMap mMap;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_map_pos);

        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        if (mapFragment != null) {
            mapFragment.getMapAsync(this);
        }
    }

    @Override
    public void onMapReady(@NonNull GoogleMap googleMap) {
        mMap = googleMap;
        // 新增一個標記在台北 101
        LatLng taipei101 = new LatLng(25.033671, 121.564427);
        //mMap.addMarker(new MarkerOptions().position(taipei101).title("台北101"));
        mMap.moveCamera(CameraUpdateFactory.newLatLngZoom(taipei101, 15));

        APIController controller = new APIController();
        controller.GetFgoMapPos(new JSONObject(), new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                JSONObject resultData = result.GetData();
                try {
                    JSONObject map_pos = resultData.getJSONObject("map_pos");
                    JSONArray data = map_pos.getJSONArray("data");

                    for (int i = 0;i < data.length();i++)
                    {
                        JSONObject json = data.getJSONObject(i);
                        Double lat = json.getDouble("lat");
                        Double lng = json.getDouble("lng");
                        LatLng pos = new LatLng(lat,lng);
                        mMap.addMarker(new MarkerOptions().position(pos));
                    }

                } catch (JSONException e) {
                    mLog.e(e);
                }
            }
        });
    }
}