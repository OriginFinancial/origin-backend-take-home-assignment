package com.useorigin.insurance.api.risk.domain.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.*;
import org.springframework.stereotype.Service;

@Service
public class RiskScoreService {

    public RiskScore calculate(RiskProfileCreationCommand command) {

        Risk riskDefault = new RiskDefault();
        riskDefault = new RiskScoreVehicleProducedIn5Years(riskDefault);
        riskDefault = new RiskScoreMarried(riskDefault);
        riskDefault = new RiskScoreWithDependents(riskDefault);
        riskDefault = new RiskScoreHouseMortgaged(riskDefault);
        riskDefault = new RiskScoreIncomeAbove200K(riskDefault);
        riskDefault = new RiskScoreBetween30To40(riskDefault);
        riskDefault = new RiskScoreUnder30(riskDefault);

        return riskDefault.createRiskScore(command);
    }
}
