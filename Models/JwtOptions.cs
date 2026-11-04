using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SystemBank.Models;
using SystemBank;
namespace SystemBank.Models;
    public class JwtOptions
    {
        public const string name = "Jwt";
        [Required]
        [MinLength(32,ErrorMessage = "password too short"),MaxLength(500, ErrorMessage = "password too very big!")]
        public string key {get;set;} = string.Empty;
        public int ExpiresHours { get; set; } = 24;
    }