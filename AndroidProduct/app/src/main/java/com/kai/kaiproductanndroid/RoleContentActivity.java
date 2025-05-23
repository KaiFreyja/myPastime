package com.kai.kaiproductanndroid;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import com.Tools.API.APIResult;
import com.Tools.API.BaseAPICallBack;
import com.Tools.mLog;
import com.github.mikephil.charting.charts.LineChart;
import com.github.mikephil.charting.components.XAxis;
import com.github.mikephil.charting.components.YAxis;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.highlight.Highlight;
import com.github.mikephil.charting.interfaces.datasets.ILineDataSet;
import com.github.mikephil.charting.listener.OnChartValueSelectedListener;
import com.squareup.picasso.Picasso;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import androidx.recyclerview.widget.RecyclerView;
import androidx.viewpager2.widget.ViewPager2;

public class RoleContentActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_role_content);

        ImageView img = findViewById(R.id.img_role_centent);
        TextView tvName = findViewById(R.id.tv_role_content_name);
        TextView tvDescription = findViewById(R.id.tv_role_content_description);
        TextView tvGender = findViewById(R.id.tv_role_content_genter);
        TextView tvProession = findViewById(R.id.tv_role_content_pression);

        Intent intent = getIntent();
        String data = intent.getStringExtra("data");
        try {
            JSONObject json = new JSONObject(data);
            String rid = json.getString("rid");
            String name = json.getString("name");
            String description = json.getString("description");
            String gid = json.getString("gid");
            String pid = json.getString("pid");

            tvName.setText(name);
            tvDescription.setText(description);
            tvGender.setText(gid);
            tvProession.setText(pid);
            String imageUrl = json.getString("url");
            Picasso.get()
                    .load(imageUrl)
                    .into(img);

            createImages(rid);
            getLevelAttr(rid);

        } catch (JSONException e) {
            mLog.e(e);
        }
    }

    private void createImages(String rid)
    {
        APIController controller = new APIController();
        JSONObject input = new JSONObject();
        try {
            input.put("rid",rid);
        } catch (JSONException e) {
            mLog.e(e);
        }
        controller.GetFgoRoleResourceImage(input, new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                JSONObject json = result.GetData();
                try {
                    JSONObject role_resource = json.getJSONObject("role_resource");
                    JSONArray data = role_resource.getJSONArray("data");
                    List<String> urls = new ArrayList<>();
                    for (int i = 0;i < data.length();i++)
                    {
                        JSONObject oneData = data.getJSONObject(i);
                        String url = oneData.getString("url");
                        urls.add(url);
                    }

                    ViewPager2 viewPager = findViewById(R.id.viewPager);
                    ImagePagerAdapter adapter = new ImagePagerAdapter(urls);
                    viewPager.setAdapter(adapter);

// 自動切換
                    Handler handler = new Handler(Looper.getMainLooper());
                    Runnable runnable = new Runnable() {
                        @Override
                        public void run() {
                            int current = viewPager.getCurrentItem();
                            int next = (current + 1) % urls.size();
                            viewPager.setCurrentItem(next, true);
                            handler.postDelayed(this, 3000); // 每 3 秒切換一次
                        }
                    };
                    handler.postDelayed(runnable, 3000);


                } catch (JSONException e) {
                    mLog.e(e);
                }
            }
        });
    }

    private void getLevelAttr(String rid) throws JSONException {
        LineChart lineChart = findViewById(R.id.lineChart);
        TextView tv = findViewById(R.id.lineChartNum);
        lineChart.setOnChartValueSelectedListener(new OnChartValueSelectedListener() {
            @Override
            public void onValueSelected(Entry e, Highlight h) {
                tv.setText("等級 : " + e.getX() + " " + e.getY());
            }

            @Override
            public void onNothingSelected() {
                tv.setText("");
            }
        });

        APIController controller = new APIController();
        JSONObject input = new JSONObject();
        input.put("rid",rid);
        controller.GetFgoRoleLevelAttr(input, new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                JSONObject json = result.GetData();
                ArrayList<Integer> atks = new ArrayList<Integer>();
                ArrayList<Integer> hps = new ArrayList<Integer>();
                try {
                    JSONObject level_attr = json.getJSONObject("level_attr");
                    JSONArray data = level_attr.getJSONArray("data");
                    for (int i = 0;i < data.length();i++)
                    {
                        JSONObject item = data.getJSONObject(i);
                        atks.add(item.getInt("atk"));
                        hps.add(item.getInt("hp"));
                    }
                }
                catch (Exception e)
                {
                    mLog.e(e);
                }

                // 準備第一組資料
                List<Entry> entries_hp = new ArrayList<>();
                for (int i = 0;i<hps.size();i++)
                {
                    entries_hp.add(new Entry(i,hps.get(i)));
                }
                // 準備第二組資料
                List<Entry> entries_atk = new ArrayList<>();
                for (int i = 0;i<atks.size();i++)
                {
                    entries_atk.add(new Entry(i,atks.get(i)));
                }

                LineDataSet dataSet1 = new LineDataSet(entries_hp, "HP"); // "數據集一" 是這條折線的標籤
                dataSet1.setColor(Color.BLUE);
                dataSet1.setValueTextColor(Color.BLACK);
                dataSet1.setLineWidth(1f);
                dataSet1.setCircleRadius(1f);
                dataSet1.setCircleColor(Color.BLUE);

                LineDataSet dataSet2 = new LineDataSet(entries_atk, "ATK"); // "數據集二" 是這條折線的標籤
                dataSet2.setColor(Color.RED);
                dataSet2.setValueTextColor(Color.BLACK);
                dataSet2.setLineWidth(1f);
                dataSet2.setCircleRadius(1f);
                dataSet2.setCircleColor(Color.RED);

                // 將兩個 DataSet 添加到一個 List 中
                List<ILineDataSet> dataSets = new ArrayList<>();
                dataSets.add(dataSet1);
                dataSets.add(dataSet2);
                // 創建 LineData 物件
                LineData lineData = new LineData(dataSets);
                // 將 LineData 設定給 LineChart
                lineChart.setData(lineData);
                // 可選：設置圖表的描述文字 (預設會顯示在右下角)
                lineChart.getDescription().setText("範例折線圖");
                lineChart.getDescription().setTextColor(Color.GRAY);
                // 可選：啟用觸控手勢
                lineChart.setTouchEnabled(true);
                lineChart.setDragEnabled(true);
                lineChart.setScaleEnabled(true);
                // 可選：啟用縮放時的動畫
                lineChart.animateX(1000);

                // 獲取 X 軸
                XAxis xAxis = lineChart.getXAxis();
                // 設置 X 軸的最大值
                xAxis.setAxisMaximum(atks.size());
                // 獲取左邊的 Y 軸
                YAxis leftAxis = lineChart.getAxisLeft();
                // 設置 Y 軸的最大值
                leftAxis.setAxisMaximum(2000);
                lineChart.getAxisRight().setAxisMaximum(2000);

                // 刷新圖表以顯示資料
                lineChart.invalidate();
            }
        });
    }

    public class ImagePagerAdapter extends RecyclerView.Adapter<ImagePagerAdapter.ViewHolder> {
        private List<String> imageList;

        public ImagePagerAdapter(List<String> imageList) {
            this.imageList = imageList;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            ImageView imageView = new ImageView(parent.getContext());
            imageView.setLayoutParams(new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MATCH_PARENT,
                    ViewGroup.LayoutParams.MATCH_PARENT
            ));
            imageView.setScaleType(ImageView.ScaleType.FIT_CENTER);
            return new ViewHolder(imageView);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            ImageView imageView = (ImageView) holder.itemView;

            Picasso.get()
                    .load(imageList.get(position))
                    .into(imageView);
        }

        @Override
        public int getItemCount() {
            return imageList.size();
        }

        class ViewHolder extends RecyclerView.ViewHolder {
            public ViewHolder(@NonNull View itemView) {
                super(itemView);
            }
        }
    }
}