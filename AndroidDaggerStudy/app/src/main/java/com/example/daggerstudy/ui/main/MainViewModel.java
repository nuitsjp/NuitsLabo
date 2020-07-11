package com.example.daggerstudy.ui.main;

import androidx.lifecycle.ViewModel;

import com.example.daggerstudy.infrastructure.LoginRetrofitService;

import javax.inject.Inject;

public class MainViewModel extends ViewModel {
    @Inject
    public MainViewModel(LoginRetrofitService loginRetrofitService)
    {

    }
}
