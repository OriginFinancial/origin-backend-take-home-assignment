package com.useorigin.insurance.api.risk.infrastructure.web.out;

import com.useorigin.insurance.api.risk.domain.ScoreType;

public class RiskProfileResource {

    private final ScoreType auto;
    private final ScoreType disability;
    private final ScoreType home;
    private final ScoreType life;

    public RiskProfileResource(ScoreType auto, ScoreType disability, ScoreType home, ScoreType life) {
        this.auto = auto;
        this.disability = disability;
        this.home = home;
        this.life = life;
    }

    public ScoreType getAuto() {
        return auto;
    }

    public ScoreType getDisability() {
        return disability;
    }

    public ScoreType getHome() {
        return home;
    }

    public ScoreType getLife() {
        return life;
    }

}
