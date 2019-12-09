package com.sparkerz.pakfarmers.dialogs;

import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;

import com.sparkerz.pakfarmers.R;
import com.sparkerz.pakfarmers.SelectAdType;

import androidx.annotation.NonNull;

public class RegisterDialog extends Dialog implements View.OnClickListener {
    private Button btnRegister;
    private EditText edtName;
    private EditText edtAddress;
    private EditText edtPhone;
    private Spinner spinCities;
    private Button btnCancel;

    public RegisterDialog(@NonNull Context context) {
        super(context);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.layout_register_dialog);
        setCancelable(true);
        getWindow().setLayout(ViewGroup.LayoutParams.FILL_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        edtName=findViewById(R.id.edtName);
        edtAddress=findViewById(R.id.edtAddress);
        edtPhone=findViewById(R.id.edtPhone);
        btnRegister=findViewById(R.id.btnRegister);
        btnRegister.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                getContext().startActivity(new Intent(getContext(), SelectAdType.class));
                dismiss();
            }
        });
        btnCancel=findViewById(R.id.btnCancel);
        btnCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dismiss();
            }
        });
    }

    @Override
    public void onClick(View view) {

    }
}
