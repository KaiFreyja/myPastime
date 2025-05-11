package com.kai.kaiproductanndroid;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.graphics.Color;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.Tools.API.APIResult;
import com.Tools.API.BaseAPICallBack;
import com.Tools.mLog;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class AiTalkMainActivity extends AppCompatActivity {

    RecyclerView content_rv;
    RecyclerView left_rv;
    EditText editText;
    JSONObject selectTalkGroupData;
    LeftMenuAdapter left_adapter;
    ContentAdapter adapter;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ai_talk_main);

        Button btn_new_ask = findViewById(R.id.btn_new_ask);
        btn_new_ask.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                newAskGroup();
            }
        });

        Button btn_ask = findViewById(R.id.btn_ask);
        btn_ask.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                sendAsk();
            }
        });
        editText = findViewById(R.id.edit_ask);

        left_rv = findViewById(R.id.rv_left_menu);
        left_rv.setLayoutManager(new LinearLayoutManager(this));

        content_rv = findViewById(R.id.rv_talk_content);
        content_rv.setLayoutManager(new LinearLayoutManager(this));

        left_adapter = new LeftMenuAdapter(new JSONArray());
        left_rv.setAdapter(left_adapter);
        updateLeftMenu();
        adapter = new ContentAdapter(new JSONArray());
        content_rv.setAdapter(adapter);
    }

    private void updateLeftMenu()
    {
        APIController controller = new APIController();
        JSONObject input = new JSONObject();
        try {
            input.put("uid",GlobalData.uid);
        } catch (JSONException e) {
            mLog.e(e);
        }
        controller.GetAiTalkGroup(input, new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                JSONObject json = result.GetData();
                try {
                    JSONObject talk_group = json.getJSONObject("talk_group");
                    JSONArray data = talk_group.getJSONArray("data");
                    left_adapter = new LeftMenuAdapter(data);
                    left_adapter.action = new ItemAction() {
                        @Override
                        public void onItemClick(JSONObject json) {
                            showContent(json);
                        }
                    };
                    left_adapter.selectIndex = data.length() - 1;
                    showContent(data.getJSONObject(left_adapter.selectIndex));
                    left_rv.setAdapter(left_adapter);

                } catch (JSONException e) {
                    mLog.e(e);
                }
            }
        });
    }

    private void showContent(JSONObject json)
    {
        selectTalkGroupData = json;
        mLog.d(""+json);
        APIController controller = new APIController();
        controller.GetAiTalkContent(json, new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                JSONObject json = result.GetData();

                try {
                    JSONObject talk_content = json.getJSONObject("talk_content");
                    JSONArray data = talk_content.getJSONArray("data");
                    mLog.d(""+data);
                    adapter.array = data;
                    adapter.notifyDataSetChanged();;
                } catch (JSONException e) {
                    mLog.e(e);
                }
            }
        });
    }

    private void newAskGroup() {
        selectTalkGroupData = null;
        adapter.array = new JSONArray();
        adapter.notifyDataSetChanged();

        left_adapter.selectIndex = -1;
        left_adapter.notifyDataSetChanged();
    }

    private void sendAsk()
    {
        String text = editText.getText().toString();

        if(text.isEmpty())
        {
            return;
        }

        APIController controller = new APIController();
        JSONObject input = new JSONObject();

        JSONObject askShowData = new JSONObject();

        try {
            if(selectTalkGroupData != null) {
                String tgid = selectTalkGroupData.getString("tgid");
                input.put("tgid", tgid);
                askShowData.put("tgid",tgid);
            }
            input.put("uid",GlobalData.uid);
            input.put("ask", text);
            askShowData.put("talker","user");
            askShowData.put("content",text);
        } catch (JSONException e) {
            mLog.e(e);
        }
        editText.setText("");


        adapter.array.put(askShowData);
        adapter.notifyDataSetChanged();
        content_rv.scrollToPosition(adapter.getItemCount() - 1);

        controller.AskAiTalk(input, new BaseAPICallBack() {
            @Override
            public void CallBack(APIResult result) {
                JSONObject json = result.GetData();
                JSONObject talk_group_ask = null;
                try {
                    talk_group_ask = json.getJSONObject("talk_group_ask");
                    if(selectTalkGroupData == null)
                    {
                        selectTalkGroupData = new JSONObject();
                        selectTalkGroupData.put("tgid",talk_group_ask.getString("tgid"));
                        updateLeftMenu();
                    }
                } catch (JSONException e) {
                    mLog.e(e);
                }
                adapter.array.put(talk_group_ask);
                adapter.notifyDataSetChanged();
            }
        });

    }


    public interface ItemAction
    {
        void onItemClick(JSONObject json);
    }
    public class LeftMenuAdapter extends RecyclerView.Adapter<LeftMenuAdapter.ViewHolder>
    {
        public int selectIndex = -1;

        public ItemAction action;

        private JSONArray array = new JSONArray();

        public LeftMenuAdapter(JSONArray data)
        {
            this.array = data;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_ai_talk_left,parent,false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            String name = null;
            JSONObject json = new JSONObject();
            try {
                json = array.getJSONObject(position);
                name = json.getString("name");
            } catch (JSONException e) {
                mLog.e(e);
            }

            holder.tv.setTextColor(position == selectIndex?Color.RED:Color.BLACK);
            holder.tv.setText(name);
            holder.json = json;

        }

        @Override
        public int getItemCount() {
            return array.length();
        }

        public class ViewHolder extends RecyclerView.ViewHolder
        {
            TextView tv;
            JSONObject json = new JSONObject();
            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                tv = itemView.findViewById(R.id.tv_item_text);

                itemView.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        for(int i = 0;i < array.length();i++)
                        {
                            try {
                                if(array.getJSONObject(i).getString("tgid") == json.getString("tgid"))
                                {
                                    selectIndex = i;
                                }
                            } catch (JSONException e) {
                                mLog.e(e);
                            }
                        }
                        notifyDataSetChanged();
                        action.onItemClick(json);
                    }
                });
            }
        }
    }

    public class ContentAdapter extends RecyclerView.Adapter<ContentAdapter.ViewHolder>
    {
        public JSONArray array = new JSONArray();

        public  ContentAdapter(JSONArray data)
        {
            array = data;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_ai_talk_content,parent,false);
            return new ContentAdapter.ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            String content = "";
            String talker = "";
            try {
                JSONObject json = array.getJSONObject(position);
                content = json.getString("content");
                talker = json.getString("talker");
            } catch (JSONException e) {
                mLog.e(e);
            }
            holder.tv_talker.setText(talker + ":");
            holder.tv.setText(content);
        }

        @Override
        public int getItemCount() {
            return array.length();
        }

        public class ViewHolder extends RecyclerView.ViewHolder
        {
            public TextView tv_talker;
            public TextView tv;
            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                tv = itemView.findViewById(R.id.tv_content);
                tv_talker = itemView.findViewById(R.id.tv_talker);
            }
        }
    }
}