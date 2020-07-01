package com.useorigin.insurance.api.risk.application.service;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.domain.Score;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.springframework.stereotype.Service;

@Service
public class RiskService implements RiskProfileUseCase {

    private RiskProfileResource riskProfileResource;

    public RiskProfileResource calculate(RiskProfileCreationCommand command) {

        Integer score = command.getRiskScore();

        if (command.getIncome() == 0 && (command.getHouse() == null || command.getHouse() == null))
            riskProfileResource = new RiskProfileResource(Score.INELIGIBLE, Score.INELIGIBLE, Score.INELIGIBLE, Score.INELIGIBLE);

        return riskProfileResource;
    }
}
