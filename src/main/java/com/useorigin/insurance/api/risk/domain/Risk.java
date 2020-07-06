package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public interface Risk {

    RiskScore createRiskScore(RiskProfileCreationCommand command);

    RiskScore updateRiskScore(RiskScore riskScore);
}