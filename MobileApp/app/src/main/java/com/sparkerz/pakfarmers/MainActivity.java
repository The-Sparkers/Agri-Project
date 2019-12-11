package com.sparkerz.pakfarmers;

import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.View;

import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.tabs.TabLayout;
import com.sparkerz.pakfarmers.fragments.buyfragment.BuyAdFragment;
import com.sparkerz.pakfarmers.fragments.buyfragment.dummy.DummyContent;
import com.sparkerz.pakfarmers.fragments.sellfragment.SellAdFragment;
import com.sparkerz.pakfarmers.ui.main.SectionsPagerAdapter;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.viewpager.widget.ViewPager;

public class MainActivity extends AppCompatActivity implements BuyAdFragment.OnListFragmentInteractionListener, SellAdFragment.OnListFragmentInteractionListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initToolbar();
        SectionsPagerAdapter sectionsPagerAdapter = new SectionsPagerAdapter(this, getSupportFragmentManager());
        ViewPager viewPager = findViewById(R.id.view_pager);
        viewPager.setAdapter(sectionsPagerAdapter);
        TabLayout tabs = findViewById(R.id.tabs);
        tabs.setupWithViewPager(viewPager);
        FloatingActionButton fab = findViewById(R.id.fab);

        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                  //      .setAction("Action", null).show();
                startActivity(new Intent(MainActivity.this,ChooseBuySell.class));
            }
        });

    }

    private void initToolbar(){
        Toolbar toolbar =findViewById(R.id.toolbar);
        toolbar.setTitle(R.string.app_name);
        setSupportActionBar(toolbar);
        //Objects.requireNonNull(getSupportActionBar()).setTitle(R.string.app_name);

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public void onListFragmentInteraction(DummyContent.DummyItem item) {

    }

    @Override
    public void onListFragmentInteraction(com.sparkerz.pakfarmers.fragments.sellfragment.dummy.DummyContent.DummyItem item) {

    }
}