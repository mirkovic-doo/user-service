﻿using FirebaseAdmin;
using FirebaseAdmin.Auth;
using UserService.Contracts.Constants;
using UserService.Contracts.Services;
using UserService.Domain;

namespace UserService.Infrastructure.Services;

public class FirebaseAuthenticationProviderService : IAuthenticationProviderService
{
    private readonly FirebaseApp firebaseApp;

    public FirebaseAuthenticationProviderService(FirebaseApp firebaseApp)
    {
        this.firebaseApp = firebaseApp;
    }

    public async Task UpdateClaimsAsync(User user)
    {
        var firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        var firebaseUser = await firebaseAuth.GetUserAsync(user.FirebaseId);

        var claims = firebaseUser.CustomClaims.Select(dict => dict).ToDictionary(pair => pair.Key, pair => pair.Value);

        claims[CustomClaims.BIEId] = user.Id;
        claims[CustomClaims.IsGuest] = user.IsGuest.ToString();

        await firebaseAuth.SetCustomUserClaimsAsync(user.FirebaseId, claims);
    }
}
