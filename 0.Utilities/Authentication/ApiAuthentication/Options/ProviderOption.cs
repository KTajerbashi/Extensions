﻿using System.Security.Claims;

namespace ApiAuthentication.Options;

public sealed class ProviderOption
{
    public bool Enabled { get; set; } = true;
    public bool IgnoreSSL { get; set; } = false;
    public string Scheme { get; set; } = default!;
    public string Authority { get; set; } = default!;
    public string? HttpClientFactoryName { get; set; }
    public TokenType TokenTypeSupport { get; set; } = TokenType.Jwt;
    public JwtTokenConfigOption JwtTokenConfig { get; set; } = new();
    public RefrenceTokenConfigOption RefrenceTokenConfig { get; set; } = new();
    public string UserIdentifierClaimType { get; set; } = ClaimTypes.NameIdentifier;
    public RegisterUserInfoClaimsOption RegisterUserInfoClaims { get; set; } = new();
    public List<UserClaimTypeMapRuleOption> UserClaimTypeMapRules { get; set; } = [];
    public EndpointsPathOption? EndpointsPath { get; set; } = null;
    public int Priority { get; set; } = 1;
}
