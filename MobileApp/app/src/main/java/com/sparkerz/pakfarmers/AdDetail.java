package com.sparkerz.pakfarmers;

import android.os.Bundle;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;

public class AdDetail extends AppCompatActivity {


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ad_detail);
        initToolbar();

    }

    private void initToolbar(){
        ActionBar toolbar=  getSupportActionBar();
        if (toolbar != null) {
            toolbar.setTitle(R.string.text_view_details_btn);
            toolbar.setDisplayHomeAsUpEnabled(true);
        }
    }



    @Override
    public boolean onSupportNavigateUp() {

        finish();
        return true;
    }
}
