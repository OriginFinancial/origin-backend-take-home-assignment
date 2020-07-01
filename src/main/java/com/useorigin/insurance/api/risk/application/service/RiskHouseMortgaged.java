package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.OwnershipStatus;
import com.useorigin.insurance.api.risk.domain.Risk;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public class RiskHouseMortgaged extends RiskDecorator {

    private static final int HOUSE_MORTGAGED_RISK_ADDITION = 1;

    public RiskHouseMortgaged(Risk risk) {
        super(risk);
    }

    @Override
    public RiskScore calculateScore(RiskScore score, RiskProfileCreationCommand command) {

        if (command.getHouse() != null) {
            if (command.getHouse().getOwnershipStatus().equals(OwnershipStatus.MORTGAGED)) {
                RiskScore scoreHouseMortgaged = createScoreHouseMortgaged(score);
                return risk.calculateScore(scoreHouseMortgaged, command);
            }
        }

        return risk.calculateScore(score, command);
    }

    @Override
    public RiskProfileResource createProfile(RiskProfileResource profile, RiskProfileCreationCommand command) {
        return null;
    }

    private RiskScore createScoreHouseMortgaged(RiskScore defaultScore) {
        return RiskScore.of(defaultScore.getScoreAuto(),
                defaultScore.getScoreDisability().add(HOUSE_MORTGAGED_RISK_ADDITION),
                defaultScore.getScoreHome().add(HOUSE_MORTGAGED_RISK_ADDITION),
                defaultScore.getScoreLife());
    }
}