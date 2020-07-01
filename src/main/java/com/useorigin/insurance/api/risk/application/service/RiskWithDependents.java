package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.Risk;
import com.useorigin.insurance.api.risk.domain.RiskDecorator;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;

public class RiskWithDependents extends RiskDecorator {

    private static final int DEPENDENTS_RISK_ADDITION = 1;

    public RiskWithDependents(Risk risk) {
        super(risk);
    }

    @Override
    public RiskScore calculateScore(RiskScore score, RiskProfileCreationCommand command) {
        if (command.getDependents() > 0) {
            RiskScore scoreDependents = createScoreDependents(score);
            return risk.calculateScore(scoreDependents, command);
        }
        return risk.calculateScore(score, command);
    }

    @Override
    public RiskProfileResource createProfile(RiskProfileResource profile, RiskProfileCreationCommand command) {
        return null;
    }

    private RiskScore createScoreDependents(RiskScore defaultScore) {
        return RiskScore.of(defaultScore.getScoreAuto(),
                defaultScore.getScoreDisability().add(DEPENDENTS_RISK_ADDITION),
                defaultScore.getScoreHome(),
                defaultScore.getScoreLife().add(DEPENDENTS_RISK_ADDITION));
    }
}
