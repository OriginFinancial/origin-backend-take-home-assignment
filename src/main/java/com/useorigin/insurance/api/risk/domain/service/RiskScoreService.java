package com.useorigin.insurance.api.risk.domain.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.RiskScoreBetween30To40;
import com.useorigin.insurance.api.risk.domain.RiskScoreUnder30;
import com.useorigin.insurance.api.risk.domain.Risk;
import com.useorigin.insurance.api.risk.domain.RiskDefault;
import com.useorigin.insurance.api.risk.domain.RiskScore;
import org.springframework.stereotype.Service;

@Service
public class RiskScoreService {

    public RiskScore calculate(RiskProfileCreationCommand command) {

        Risk riskDefault = new RiskDefault();
        riskDefault = new RiskScoreUnder30(riskDefault);
        riskDefault = new RiskScoreBetween30To40(riskDefault);

        return riskDefault.createRiskScore(command);
    }
}
