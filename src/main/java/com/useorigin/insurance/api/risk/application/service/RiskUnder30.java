package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.*;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public class RiskUnder30 extends RiskDecorator {

    private static final int UNDER_30_RISK_DEDUCTION = 2;

    public RiskUnder30(Risk risk) {
        super(risk);
    }

    @Override
    public RiskScore calculateScore(RiskScore score, RiskProfileCreationCommand command) {

        if (command.getAge() < 30) {
            RiskScore scoreUnder30 = createScoreUnder30(score);
            return risk.calculateScore(scoreUnder30, command);
        }
        return risk.calculateScore(score, command);

    }

    private RiskScore createScoreUnder30(RiskScore defaultScore) {
        return RiskScore.of(defaultScore.getScoreAuto().deduct(UNDER_30_RISK_DEDUCTION),
                defaultScore.getScoreDisability().deduct(UNDER_30_RISK_DEDUCTION),
                defaultScore.getScoreHome().deduct(UNDER_30_RISK_DEDUCTION),
                defaultScore.getScoreLife().deduct(UNDER_30_RISK_DEDUCTION));
    }

    @Override
    public RiskProfileResource createProfile(RiskProfileResource profile, RiskProfileCreationCommand command) {
        return null;
    }
}
