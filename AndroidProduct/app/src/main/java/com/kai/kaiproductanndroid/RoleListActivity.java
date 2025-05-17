package com.kai.kaiproductanndroid;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.Tools.API.APIResult;
import com.Tools.API.BaseAPICallBack;
import com.Tools.mLog;
import com.squareup.picasso.Picasso;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class RoleListActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_role_list);


        RecyclerView recyclerView = findViewById(R.id.rv_role_list);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
        APIController controller = new APIController();
        controller.GetFgoGetRole(new JSONObject(), new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {

                JSONObject json = result.GetData();
                mLog.d(json.toString());

                try {
                    JSONObject role = json.getJSONObject("role");
                    JSONArray data = role.getJSONArray("data");
                    RoleLidtAdapter adapter = new RoleLidtAdapter(data);
                    recyclerView.setAdapter(adapter);
                } catch (Exception e) {
                    mLog.e(e);
                }

            }
        });
    }

    public interface ViewHolderCLick
    {
        void click();
    }

    public class  RoleLidtAdapter extends RecyclerView.Adapter<RoleLidtAdapter.ViewHolder>
    {

        JSONArray list = new JSONArray();
        public  RoleLidtAdapter(JSONArray list)
        {
            mLog.d(list.toString());
            this.list = list;
        }


        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View itemView = LayoutInflater.from(parent.getContext())
                    .inflate(R.layout.layout_role_list_item, parent, false);
            return new ViewHolder(itemView);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            String rid = "";
            String name = "";
            String url = "";
            JSONObject json = new JSONObject();
            try {
                json = list.getJSONObject(position);
                rid = json.getString("rid");
                name = json.getString("name");
                url = json.getString("url");
            } catch (Exception e) {
                mLog.e(e);
            }

            holder.tvName.setText(name);
            String imageUrl = url;
            Picasso.get()
                    .load(imageUrl)
                    .into(holder.imgRole);

            JSONObject finalJson = json;
            holder.click = new ViewHolderCLick() {
                @Override
                public void click() {
                    Intent intent = new Intent(RoleListActivity.this,RoleContentActivity.class);
                    intent.putExtra("data", finalJson.toString());
                    startActivity(intent);
                }
            };
        }

        @Override
        public int getItemCount() {
            return list.length();
        }

        public class ViewHolder extends RecyclerView.ViewHolder {;
            public ImageView imgRole;
            public TextView tvName;
            public ViewHolderCLick click;
            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                imgRole = itemView.findViewById(R.id.imgRole);
                tvName = itemView.findViewById(R.id.tvName);
                itemView.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        click.click();
                    }
                });
            }
        }
    }
}