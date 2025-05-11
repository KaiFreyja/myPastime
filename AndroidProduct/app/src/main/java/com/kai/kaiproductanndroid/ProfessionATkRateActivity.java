package com.kai.kaiproductanndroid;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.os.Bundle;
import android.os.Handler;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.Tools.API.APIResult;
import com.Tools.API.BaseAPICallBack;
import com.Tools.mLog;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class ProfessionATkRateActivity extends AppCompatActivity {

    private RecyclerView recyclerViewGrid;
    private GridAdapter gridAdapter;
    private List<Cell> cellDataList;
    JSONArray profession = new JSONArray();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profession_atk_rate);

        recyclerViewGrid = findViewById(R.id.recyclerViewTable); // 確保你的 RecyclerView 的 ID 是 recyclerViewTable

        APIController controller = new APIController();
        controller.GetFgoGetProfession(new JSONObject(), new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                try {
                    JSONObject json = result.GetData().getJSONObject("profession");
                    profession = json.getJSONArray("data");
                    controller.GetFgoProfessionAtkRate(new JSONObject(), new BaseAPICallBack() {
                        @Override
                        public void CallBack(APIResult result) {
                            createTableView();
                            getData();
                        }
                    });

                } catch (Exception e) {
                    mLog.e(e);
                }
            }
        });
    }

    private void createTableView() {

        int GRID_SIZE = profession.length() + 1;

        GridLayoutManager layoutManager = new GridLayoutManager(ProfessionATkRateActivity.this, GRID_SIZE); // 設定列數為 12
        recyclerViewGrid.setLayoutManager(layoutManager);
        // 初始化資料
        cellDataList = new ArrayList<>();
        for (int i = 0; i < GRID_SIZE * GRID_SIZE; i++) {
            cellDataList.add(new Cell("0")); // 初始值為 0
        }
        gridAdapter = new GridAdapter(cellDataList, GRID_SIZE);
        recyclerViewGrid.setAdapter(gridAdapter);
        gridAdapter.updateCell(0, 0, "攻擊方/守備方");
        for (int i = 0; i < profession.length(); i++) {
            try {
                JSONObject data = profession.getJSONObject(i);
                String name = data.getString("name");
                gridAdapter.updateCell(i + 1, 0, name);
                gridAdapter.updateCell(0, i + 1, name);
            } catch (JSONException e) {
                mLog.e(e);
            }
        }
    }

    private void getData() {
        APIController controller = new APIController();
        controller.GetFgoProfessionAtkRate(new JSONObject(), new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                try {
                    JSONObject json = result.GetData().getJSONObject("profession_atk_rate");
                    JSONArray profession_atk_rate = json.getJSONArray("data");

                    for (int i = 0;i < profession_atk_rate.length();i++)
                    {
                        JSONObject data = profession_atk_rate.getJSONObject(i);
                        String atk_pid = data.getString("atk_pid");
                        String def_pid = data.getString("def_pid");
                        int atkIndex = 0;
                        int defIndex = 0;
                        String value = data.getString("rate");
                        for (int j = 0;j < profession.length();j++)
                        {
                            String pid = profession.getJSONObject(j).getString("pid");
                            if(atk_pid == pid)
                            {
                                atkIndex = j;
                            }
                            if(def_pid == pid)
                            {
                                defIndex = j;
                            }
                        }
                        gridAdapter.updateCell(atkIndex + 1,defIndex + 1,value);
                    }

                } catch (JSONException e) {
                    mLog.e(e);
                }
            }
        });
    }

    public class Cell {
        private String value;

        public Cell(String value) {
            this.value = value;
        }

        public String getValue() {
            return value;
        }

        public void setValue(String value) {
            this.value = value;
        }
    }
    public class GridAdapter extends RecyclerView.Adapter<GridAdapter.ViewHolder> {

        private List<Cell> cellList;
        private int columnCount;

        public GridAdapter(List<Cell> cellList, int columnCount) {
            this.cellList = cellList;
            this.columnCount = columnCount;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext())
                    .inflate(R.layout.item_cell, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            Cell currentCell = cellList.get(position);
            holder.textViewCellValue.setText(String.valueOf(currentCell.getValue()));
        }

        @Override
        public int getItemCount() {
            return cellList.size(); // 總共有 12 * 12 = 144 個單元格
        }

        // 更新特定單元格資料的方法
        public void updateCell(int row, int col, String newValue) {
            int index = row * columnCount + col;
            if (index >= 0 && index < cellList.size()) {
                cellList.get(index).setValue(newValue);
                notifyItemChanged(index);
            }
        }

        public class ViewHolder extends RecyclerView.ViewHolder {
            TextView textViewCellValue;

            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                textViewCellValue = itemView.findViewById(R.id.textViewCellValue);
            }
        }
    }
}