package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.MaritalStatus;
import com.useorigin.insurance.api.risk.domain.Risk;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public class RiskIsMarried extends RiskDecorator {

    private static final int MARRIED_RISK_ADDITION = 1;
    private static final int MARRIED_RISK_DEDUCTION = 1;

    public RiskIsMarried(Risk risk) {
        super(risk);
    }

    @Override
    public RiskScore calculateScore(RiskScore score, RiskProfileCreationCommand command) {
        if (command.getMaritalStatus().equals(MaritalStatus.MARRIED)) {
            RiskScore scoreMarried = createScoreMarried(score);
            return risk.calculateScore(scoreMarried, command);
        }
        return risk.calculateScore(score, command);
    }

    @Override
    public RiskProfileResource createProfile(RiskProfileResource profile, RiskProfileCreationCommand command) {
        return null;
    }

    private RiskScore createScoreMarried(RiskScore defaultScore) {
        return RiskScore.of(defaultScore.getScoreAuto(),
                defaultScore.getScoreDisability().deduct(MARRIED_RISK_DEDUCTION),
                defaultScore.getScoreHome(),
                defaultScore.getScoreLife().add(MARRIED_RISK_ADDITION));
    }
}
