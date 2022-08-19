import { Inject, Injectable, Renderer2 } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
    providedIn: 'root'
})
export class ScriptService {
    constructor(@Inject(DOCUMENT) private document: Document) { }

    /**
     * Append the JS tag to the Document Body.
     * @param renderer The Angular Renderer
     * @param src The path to the script
     * @returns the script element
     */
    public loadJsScript(renderer: Renderer2, content: string): HTMLScriptElement {
        const script = renderer.createElement('script') as HTMLScriptElement;
        script.type = 'text/javascript';
        script.innerHTML = content;
        renderer.appendChild(this.document.body, script);
        return script;
    }

    /**
     * Append the JS tag to the Document Body.
     * @param renderer The Angular Renderer
     * @param src The path to the script
     * @returns the script element
     */
     public loadExternalScript(renderer: Renderer2, link: string): HTMLScriptElement {
        const script = renderer.createElement('script') as HTMLScriptElement;
        script.type = 'text/javascript';
        script.src = link;
        renderer.appendChild(this.document.body, script);
        return script;
    }
}