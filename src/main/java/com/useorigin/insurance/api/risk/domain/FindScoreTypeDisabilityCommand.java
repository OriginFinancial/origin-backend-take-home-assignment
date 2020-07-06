package com.useorigin.insurance.api.risk.domain;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;

public class FindScoreTypeDisabilityCommand implements FindScoreTypeCommand {

    private static final int SIXTY_YEARS_OLD = 60;
    private static final int NO_INCOME = 0;

    private final ScoreToScoreTypeConverter converter;
    private final Score score;

    public FindScoreTypeDisabilityCommand(ScoreToScoreTypeConverter converter, Score score) {
        this.converter = converter;
        this.score = score;
    }

    @Override
    public ScoreType execute(RiskProfileCreationCommand command) {
        if (command.getIncome() == NO_INCOME || command.getAge() > SIXTY_YEARS_OLD)
            return ScoreType.INELIGIBLE;

        return converter.convert(score.getValue());
    }
}
