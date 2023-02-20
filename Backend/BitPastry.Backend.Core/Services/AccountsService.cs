using System;

using BitPastry.Backend.DTO.Exceptions;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using BitPastry.Backend.DTO.Models;
using Microsoft.EntityFrameworkCore;
using Molim.Backend.API.Utils.Cryptography;

namespace BitPastry.Backend.Core.Services;
public class AccountsService : BaseService
{
    public AccountsService(BaseService b) : base(b) { }

    /* ----------------------------------------------------------------------------------------- */
    public AccountProxy Authenticate(
        string? username,
        string? password,
        bool isOperatoreLogin
    ) {
        // Controllo che l'Username esista
        if (string.IsNullOrEmpty(username))
            throw AuthException.UsernameEmpty();

        // Passowrd? Nah meglio di no
        string? hashedPassword = null;
        if (!string.IsNullOrEmpty(password))
            hashedPassword = new Cryptor(BitPastryConfiguration.CryptoSecret).Hash(password);

        var account = Database.Accounts
            .Where(x =>
                x.Deleted == false &&
                x.Username.ToLower() == username.ToLower() &&
                x.Password == hashedPassword
            )
            .AsNoTracking()
            .Select(account => new AccountProxy()
            {
                ID = account.IdAccount,
                IDOperatore = account.OperatoriIdOperatore,
                Username = account.Username,
                FullName = Database.Operatoris
                    .Where(operatore => operatore.Deleted == false && operatore.IdOperatore == account.OperatoriIdOperatore)
                    .Select(x => $"{x.Nome} {x.Cognome}")
                    .FirstOrDefault()
            }
            ).SingleOrDefault();

        // Controllo che l'Account esista
        if (account == null)
            throw AuthException.AuthFail();

        // Controllo l'Account soddisfa il requisito del Login
        if (isOperatoreLogin && account.IDOperatore == null || (!isOperatoreLogin && account.IDOperatore != null))
            throw AuthException.AuthFail();

        return account;
    }
}

