package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public interface Risk {
    RiskScore calculateScore(RiskScore riskScore, RiskProfileCreationCommand command);

    RiskProfileResource createProfile(RiskProfileResource profile, RiskProfileCreationCommand command);
}
