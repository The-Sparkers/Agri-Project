package com.sparkerz.pakfarmers;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.sparkerz.pakfarmers.dialogs.RegisterDialog;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;

public class ChooseBuySell extends AppCompatActivity {

    private Button btnBuyer;
    private Button btnSeller;
    private RegisterDialog registerDialog;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_choose_buy_sell);
        initToolbar();
        btnBuyer=findViewById(R.id.btnSelectBuyer);
        btnBuyer.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                registerDialog= new RegisterDialog(ChooseBuySell.this);
                registerDialog.show();
            }
        });
        btnSeller=findViewById(R.id.btnSelectSeller);
        btnSeller.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                registerDialog= new RegisterDialog(ChooseBuySell.this);
                registerDialog.show();
            }
        });
    }
    private void initToolbar(){
        ActionBar toolbar=  getSupportActionBar();
        if (toolbar != null) {
            toolbar.setTitle("Choose Buy or Sell");
            toolbar.setDisplayHomeAsUpEnabled(true);
        }
    }

    @Override
    public void onBackPressed() {
        try{
            if (registerDialog.isShowing()){
                registerDialog.dismiss();
            }
        }
        catch (Exception ex){
            ex.printStackTrace();
        }
        super.onBackPressed();
    }
    @Override
    public boolean onSupportNavigateUp() {
        finish();
        return super.onSupportNavigateUp();
    }
}
