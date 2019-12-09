package com.sparkerz.pakfarmers;

import android.os.Bundle;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;

public class SelectAdType extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_select_ad_type);
        initToolbar();
    }
    private void initToolbar(){
        ActionBar toolbar=  getSupportActionBar();
        if (toolbar != null) {
            toolbar.setTitle(R.string.text_select_type);
            toolbar.setDisplayHomeAsUpEnabled(true);
        }
    }



    @Override
    public boolean onSupportNavigateUp() {
        finish();
        return true;
    }
}
