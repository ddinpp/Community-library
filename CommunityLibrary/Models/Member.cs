using System;

namespace CommunityLibrary.Models;

public class Member
{
    public int MemberId { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }

    public Member(int memberId, string fullName, string phone, string email, bool isActive = true)
    {
        MemberId = memberId;
        FullName = fullName;
        Phone = phone;
        Email = email;
        IsActive = isActive;
    }
}
