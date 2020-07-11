package com.example.daggerstudy.infrastructure;

import dagger.Module;
import dagger.Provides;
import retrofit2.Retrofit;

@Module
public class NetworkModule {
    @Provides
    public LoginRetrofitService provideLoginRetrofitService() {
        // Whenever Dagger needs to provide an instance of type LoginRetrofitService,
        // this code (the one inside the @Provides method) is run.
        return new LoginService();
    }
}
