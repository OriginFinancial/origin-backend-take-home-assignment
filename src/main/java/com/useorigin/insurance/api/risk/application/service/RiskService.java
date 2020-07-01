package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.*;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.springframework.stereotype.Service;

@Service
public class RiskService {

    public RiskProfileResource createProfile(RiskProfileCreationCommand command) {

        RiskProfileResource profile = new RiskProfileResource();
        RiskDecorator risk = new RiskScoreIneligible(new RiskOver60());

        return risk.createProfile(profile, command);

    }

    public RiskScore calculateScore(RiskProfileCreationCommand command) {
        RiskScore score = createDefaultScore(command.getRiskScore());
        RiskDecorator risk = new RiskScoreIneligible(new RiskOver60(new RiskUnder30(new RiskBetween30To40(new RiskIncomeAbove200K(new RiskHouseMortgaged(new RiskWithDependents(new RiskIsMarried(new RiskNewCar()))))))));
        return risk.calculateScore(score, command);
    }

    private RiskScore createDefaultScore(Integer score) {
        ScoreAuto scoreAuto = new ScoreAuto(score);
        ScoreDisability scoreDisability = new ScoreDisability(score);
        ScoreHome scoreHome = new ScoreHome(score);
        ScoreLife scoreLife = new ScoreLife(score);

        return RiskScore.of(scoreAuto, scoreDisability, scoreHome, scoreLife);
    }
}
