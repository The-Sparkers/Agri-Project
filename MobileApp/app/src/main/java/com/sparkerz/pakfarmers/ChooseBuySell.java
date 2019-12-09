package com.sparkerz.pakfarmers;

import android.os.Bundle;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;

public class ChooseBuySell extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_choose_buy_sell);
        initToolbar();
    }
    private void initToolbar(){
        ActionBar toolbar=  getSupportActionBar();
        if (toolbar != null) {
            toolbar.setTitle("Choose Buy or Sell");
            toolbar.setDisplayHomeAsUpEnabled(true);
        }
    }

    @Override
    public boolean onSupportNavigateUp() {
        finish();
        return super.onSupportNavigateUp();
    }
}
