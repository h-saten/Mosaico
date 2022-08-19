import { Injectable } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { Page, Project } from 'mosaico-project';


@Injectable({
    providedIn: 'root'
})
export class MetatagsService {
    constructor(private titleService: Title, private metaService: Meta){

    }

    public setData(page: Page, project: Project): void {
        this.titleService.setTitle(project.title);
        this.metaService.updateTag({name: 'description', content: page.shortDescription});
        this.metaService.updateTag({property: 'og:description', content: page.shortDescription});
        this.metaService.updateTag({property: 'og:title', content: project.title});
        this.metaService.updateTag({property: 'og:image', content: page.coverUrl});
        this.metaService.updateTag({property: 'og:image:secure_url', content: page.coverUrl});
        this.metaService.updateTag({name: 'twitter:image', content: page.coverUrl});
    }

    public reset(): void {
        this.titleService.setTitle('Mosaico - Tokenization and DAO');
        this.metaService.updateTag({name: 'description', content: 'Tokenizacja Twojego biznesu. Pozyskaj fundusze od tysięcy inwestorów lub inwestuj już teraz. Zyskaj społeczność i pochodzący od niej kapitał.'});
        this.metaService.updateTag({property: 'og:description', content: 'Tokenizacja Twojego biznesu. Pozyskaj fundusze od tysięcy inwestorów lub inwestuj już teraz. Zyskaj społeczność i pochodzący od niej kapitał.'});
        this.metaService.updateTag({property: 'og:title', content: 'Mosaico - Tokenization and DAO'});
        this.metaService.updateTag({property: 'og:image', content: '/assets/media/mosaico/mosaico_social.png'});
        this.metaService.updateTag({property: 'og:image:secure_url', content: '/assets/media/mosaico/mosaico_social.png'});
        this.metaService.updateTag({name: 'twitter:image', content: '/assets/media/mosaico/mosaico_social.png'});
    }
}