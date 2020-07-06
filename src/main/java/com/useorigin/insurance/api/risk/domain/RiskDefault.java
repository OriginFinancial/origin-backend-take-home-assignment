package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class RiskDefault implements Risk {

    @Override
    public RiskScore createRiskScore(RiskProfileCreationCommand command) {
        Integer score = command.getDefaultRiskScore();

        ScoreAuto scoreAuto = new ScoreAuto(score);
        ScoreDisability scoreDisability = new ScoreDisability(score);
        ScoreHome scoreHome = new ScoreHome(score);
        ScoreLife scoreLife = new ScoreLife(score);

        return RiskScore.of(scoreAuto, scoreDisability, scoreHome, scoreLife);
    }

    @Override
    public RiskScore updateRiskScore(RiskScore riskScore) {
        return riskScore;
    }
}
