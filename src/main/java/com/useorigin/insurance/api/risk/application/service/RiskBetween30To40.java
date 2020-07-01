package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.Risk;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public class RiskBetween30To40 extends RiskDecorator {

    private static final int BETWEEN_30_AND_40_RISK_DEDUCTION = 1;

    public RiskBetween30To40(Risk risk) {
        super(risk);
    }

    @Override
    public RiskScore calculateScore(RiskScore score, RiskProfileCreationCommand command) {
        if (command.getAge() >= 30 && command.getAge() < 40) {
            RiskScore scoreBetween30And40 = createScoreBetween30And40(score);
            return risk.calculateScore(scoreBetween30And40, command);
        }

        return risk.calculateScore(score, command);
    }

    private RiskScore createScoreBetween30And40(RiskScore defaultScore) {
        return RiskScore.of(defaultScore.getScoreAuto().deduct(BETWEEN_30_AND_40_RISK_DEDUCTION),
                defaultScore.getScoreDisability().deduct(BETWEEN_30_AND_40_RISK_DEDUCTION),
                defaultScore.getScoreHome().deduct(BETWEEN_30_AND_40_RISK_DEDUCTION),
                defaultScore.getScoreLife().deduct(BETWEEN_30_AND_40_RISK_DEDUCTION));
    }


    @Override
    public RiskProfileResource createProfile(RiskProfileResource profile, RiskProfileCreationCommand command) {
        return null;
    }
}
