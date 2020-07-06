package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.domain.RiskScore;
import com.useorigin.insurance.api.risk.domain.ScoreType;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.springframework.core.convert.converter.Converter;

public class RiskScoreToRiskProfileResourceConverter implements Converter<RiskScore, RiskProfileResource> {

    @Override
    public RiskProfileResource convert(RiskScore source) {
        ScoreType auto = mapScoreType(source.getScoreAuto().getScore());
        ScoreType disability = mapScoreType(source.getScoreDisability().getScore());
        ScoreType home = mapScoreType(source.getScoreHome().getScore());
        ScoreType life = mapScoreType(source.getScoreLife().getScore());

        return new RiskProfileResource(auto, disability, home, life);
    }

    private ScoreType mapScoreType(Integer score) {
        if (score <= 0)
            return ScoreType.ECONOMIC;

        if (score == 1 || score == 2)
            return ScoreType.REGULAR;

        return ScoreType.RESPONSIBLE;
    }

    private boolean isIneligible(ScoreType scoreType) {
        return scoreType.equals(ScoreType.INELIGIBLE);
    }

}
