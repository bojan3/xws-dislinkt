using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;
using Play.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Play.Catalog.Service.Models;

namespace Play.Catalog.Service.Controller
{

    [ApiController]
    // https://localhost:5002/
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IRepository<Account> accountsRepository;
        private readonly IRepository<AuthenticateResponse> responseRepository;

        public AccountController(IRepository<Account> accountsRepository, IRepository<AuthenticateResponse> responseRepository)
        {
            this.accountsRepository = accountsRepository;
            this.responseRepository = responseRepository;
        }

        [EnableCors("Policy1")]
        [HttpGet("GetAllAccounts")]
        public async Task<IEnumerable<AccountDto>> GetPublicAsync()
        {

            var accounts = (await accountsRepository.GetAllAsync())
            .Where(account => account.IsPublic)
            .Select(account => account.AsDto());

            return accounts;
        }

        [EnableCors("Policy1")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetByIdAsync(Guid id)
        {
            var account = (await accountsRepository.GetAsync(id)).AsDto();

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpPost("FollowMethod")]
        public async Task<ActionResult<Account>> FollowMethod(Guid followerID, Guid followedID)
        {

            var followerAccount = await accountsRepository.GetAsync(followerID);

            var followedAccount = await accountsRepository.GetAsync(followedID);

            if (followerID == followedID)
            {
                return BadRequest();
            }

            if (followerAccount.FollowedAccounts != null /*&& followerAccount.FollowedAccounts.Any()*/)
            {
                foreach (var linkedAccount in followerAccount.FollowedAccounts)
                {
                    if (linkedAccount.FollowedID == followedAccount.Id)
                    {
                        return BadRequest();
                    }
                }
            }

            var LinkAccount = new db_Account2Account
            {
                ID = Guid.NewGuid(),
                FollowerID = followerID,
                FollowedID = followedID,
                IsApproved = followedAccount.IsPublic
            };

            followerAccount.FollowedAccounts.Add(LinkAccount);
            followedAccount.FollowersAccounts.Add(LinkAccount);
            await accountsRepository.UpdateAsync(followedAccount);
            await accountsRepository.UpdateAsync(followerAccount);

            return NoContent();

        }

        [HttpPut("Approve")]
        public async Task<IActionResult> ApproveFollow(db_Account2Account follow)
        {
            var followerAccount = await accountsRepository.GetAsync(follow.FollowerID);
            var followedAccount = await accountsRepository.GetAsync(follow.FollowedID);

            follow.IsApproved = true;

            //Potvrdjuje kod pratioca
            foreach (var linkedAccount in followerAccount.FollowedAccounts)
            {
                if (linkedAccount.FollowedID == followedAccount.Id)
                {
                    linkedAccount.IsApproved = true;
                }
            }
            //Potvrdjuje kod pracenog profila
            foreach (var linkedAccount in followedAccount.FollowersAccounts)
            {
                if (linkedAccount.FollowerID == followerAccount.Id)
                {
                    linkedAccount.IsApproved = true;
                }

            }
            await accountsRepository.UpdateAsync(followedAccount);
            await accountsRepository.UpdateAsync(followerAccount);

            return NoContent();

        }

        [EnableCors("Policy1")]
        [HttpPost("CreateAccount")]
        public async Task<ActionResult<AccountDto>> PostAsync(CreateAccountDto createAccountDto)
        {
            var accounts = (await accountsRepository.GetAllAsync());

            foreach (var acc in accounts)
            {

                if (acc.Username == createAccountDto.Username)
                {
                    return BadRequest(ModelState);
                }
            }
            var account = new Account
            {
                Username = createAccountDto.Username,
                Email = createAccountDto.Email,
                Password = createAccountDto.Password,
                PhoneNumber = createAccountDto.PhoneNumber,
                Gender = createAccountDto.Gender,
                Biography = createAccountDto.Biography,
                IsPublic = createAccountDto.IsPublic,
                Job = createAccountDto.Job,
                Education = createAccountDto.Education,
            };

            await accountsRepository.CreateAsync(account);

            // this return route of item when it's stored
            return CreatedAtAction(nameof(GetByIdAsync), new { id = account.Id }, account);
        }

        [HttpPut("UpdateAccount/{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdatedAccountDto updateAccountDto)
        {
            var existingAccount = await accountsRepository.GetAsync(id);

            var accounts = (await accountsRepository.GetAllAsync()).Where(account => account.Id != id);

            foreach (var acc in accounts)
            {

                if (acc.Username == updateAccountDto.Username)
                {
                    return BadRequest(ModelState);
                }
            }

            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.Username = updateAccountDto.Username;
            existingAccount.Password = updateAccountDto.Password;
            existingAccount.Email = updateAccountDto.Email;
            existingAccount.PhoneNumber = updateAccountDto.PhoneNumber;
            existingAccount.Biography = updateAccountDto.Biography;
            existingAccount.Job = updateAccountDto.Job;
            existingAccount.Education = updateAccountDto.Education;
            existingAccount.IsPublic = updateAccountDto.IsPublic;

            await accountsRepository.UpdateAsync(existingAccount);

            return NoContent();
        }

        [HttpGet("isPublic/{id}")]
        public async Task<ActionResult<bool>> IsPublicAsync(Guid id)
        {
            var account = (await accountsRepository.GetAsync(id)).AsDto();

            if (account == null)
            {
                return NotFound();
            }

            return account.IsPublic;

        }

        [EnableCors("Policy1")]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(AuthenticateRequest request)
        {
            Console.WriteLine(request);
            var accounts = (await accountsRepository.GetAllAsync());

            foreach (var account in accounts)
            {

                if (account.Username == request.Username && account.Password == request.Password)
                {
                    var token = generateJwtToken(account.AsDto());
                    AuthenticateResponse autRes = new AuthenticateResponse(account.AsDto(), token);
                    await PostResponseAsync(autRes);
                    return token;
                }
            }

            return NotFound();
        }

        [HttpPost("CreateToken")]
        public async Task<ActionResult<AuthenticateResponse>> PostResponseAsync(AuthenticateResponse response)
        {
            await responseRepository.CreateAsync(response);

            return response;
        }

        public async Task<ActionResult<bool>> Authorization(Guid id, String token){
            var tokens = (await responseRepository.GetAllAsync());

            foreach(var t in tokens){
                if(id == t.Id && token == t.Token){
                    return true;
                }
            }
            return false;
        }

        private string generateJwtToken(AccountDto user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            string Secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var index = items.FindIndex(existingItem => existingItem.Id == id);

            var existingAccount = await accountsRepository.GetAsync(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            await accountsRepository.RemoveAsync(id);

            // if (index < 0)
            // {
            //     return NotFound();
            // }

            // items.RemoveAt(index);

            return NoContent();
        }
    }

}