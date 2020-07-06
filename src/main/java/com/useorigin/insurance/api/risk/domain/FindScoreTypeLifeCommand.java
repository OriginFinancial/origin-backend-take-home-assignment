package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class FindScoreTypeLifeCommand implements FindScoreTypeCommand {

    private static final int SIXTY_YEARS_OLD = 60;
    private final ScoreToScoreTypeConverter converter;
    private final Score score;

    public FindScoreTypeLifeCommand(ScoreToScoreTypeConverter converter, Score score) {
        this.converter = converter;
        this.score = score;
    }

    @Override
    public ScoreType execute(RiskProfileCreationCommand command) {
        if (command.getAge() > SIXTY_YEARS_OLD)
            return ScoreType.INELIGIBLE;

        return converter.convert(score.getValue());
    }
}
