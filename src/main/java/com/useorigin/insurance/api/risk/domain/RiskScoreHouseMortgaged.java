package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskScoreHouseMortgaged extends RiskDecorator {

    private static final int HOUSE_MORTGAGED_RISK_ADDITION = 1;

    private Risk risk;

    public RiskScoreHouseMortgaged(Risk risk) {
        this.risk = risk;
    }

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        if (command.getHouse() != null)
            if (command.getHouse().getOwnershipStatus().equals(OwnershipStatus.MORTGAGED))
                return updateRiskScore(risk.createRiskScore(command));

        return risk.createRiskScore(command);
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return RiskScore.of(riskScore.getScoreAuto(),
                riskScore.getScoreDisability().add(HOUSE_MORTGAGED_RISK_ADDITION),
                riskScore.getScoreHome().add(HOUSE_MORTGAGED_RISK_ADDITION),
                riskScore.getScoreLife());
    }
}