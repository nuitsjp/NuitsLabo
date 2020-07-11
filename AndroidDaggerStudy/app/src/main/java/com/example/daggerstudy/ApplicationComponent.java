package com.example.daggerstudy;

import com.example.daggerstudy.infrastructure.NetworkModule;
import com.example.daggerstudy.ui.main.MainFragment;

import javax.inject.Singleton;

import dagger.Component;

@Singleton
@Component(modules = NetworkModule.class)
public interface ApplicationComponent {
    void inject(MainFragment mainFragment);
}
